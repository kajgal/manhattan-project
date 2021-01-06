#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <errno.h>
#include <string.h>
#include <pthread.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <signal.h>

#define MAX_CLIENTS 10
#define BUFFER_SIZE 2048
#define NAME_LEN 32
#define fileBufferSIZE 1025
#define SIZE 1024

static _Atomic unsigned  int clientsCount = 0;
static int uid = 10;

// Client Structure
typedef struct {
    struct sockaddr_in address;
    int sockfd;
    int uid;
    char name[NAME_LEN];
} client_t;

client_t *clients[MAX_CLIENTS];

pthread_mutex_t clients_mutex = PTHREAD_MUTEX_INITIALIZER;

void str_trim_lf(char* arr, int length) {
    for(int i = 0; i < length; i++) {
        if(arr[i] == '\n') {
            arr[i] = '\0';
            break;
        }
    }
}


// function to add client to queue of clients
void queue_add(client_t *cl) {
    pthread_mutex_lock(&clients_mutex);

    for(int i = 0; i < MAX_CLIENTS; i++) {
        if(!clients[i]) {
            clients[i] = cl;
            break;
        }
    }

    pthread_mutex_unlock(&clients_mutex);
}

// function to remove client from queue
void queue_remove(int uid) {
    pthread_mutex_lock(&clients_mutex);

    for(int i = 0; i < MAX_CLIENTS; i++) {
        if(clients[i]) {
            if(clients[i]->uid == uid) {
                clients[i] = NULL;
                break;
            }
        }
    }
    pthread_mutex_unlock(&clients_mutex);
}

// function to print IP Address of client
void printIpAddress(struct sockaddr_in address) {
    printf("%d.%d.%d.%d\n",
            address.sin_addr.s_addr & 0xff,
           (address.sin_addr.s_addr & 0xff00) >> 8,
           (address.sin_addr.s_addr & 0xff00) >> 16,
           (address.sin_addr.s_addr & 0xff0000) >> 24);
}

// function to find socket by username, its used to send direct message to specific client, we pass username of client that should receive
// message, we find that user and send that message to him
void send_direct_message(char* username, char* message) {
    for(int i = 0; i < MAX_CLIENTS; i++) {
        if(clients[i]) {
            // if name matches we find that user
            if(strcmp(clients[i]->name, username) == 0) {
                if(write(clients[i]->sockfd, message, strlen(message)) < 0) {
                    printf("ERROR: write to descriptor failed\n");
                    break;
                }
            }
        }
    }
}

// getting current list of online users at the moment of connection
void current_users_list(client_t *connectedClient) {
    // here we create one big string with names of all clients
    // prefix for getting users onload
    char usersStr[10000] = "+{OnlineUsersListOnLoad}+ ";
    for(int i = 0; i < MAX_CLIENTS; i++) {
        // if client exists
        if(clients[i]) {
            // we don't add name of connectedClient nickname because we want other users
            if(strcmp(clients[i]->name, connectedClient->name) != 0) {
                // add space to split it on client side
                strcat(usersStr, clients[i]->name);
                strcat(usersStr, " ");
            }
        }
    }
    // send message to connectedClient containing usersStr which is list of clients that will be parsed on client side
    send_direct_message(connectedClient->name, usersStr);
}

// we pass here connected user and send request to every socket except connectedClient socket to add new user which is connectedClient
void update_users_list_on_new_connection(client_t *connectedClient) {
    // prefix for client add 
    char newUserStr[10000] = "+{AddNewUser}+ ";
    // information about client to add
    strcat(newUserStr, connectedClient->name);
    // adding new client
    for(int i = 0; i < MAX_CLIENTS; i++) {
        // we send that update request to every user expect the new one
        if(clients[i] && clients[i] != connectedClient) { // ?
            if(write(clients[i]->sockfd, newUserStr, strlen(newUserStr)) < 0) {
                printf("ERROR: write to descriptor failed\n");
                break;
            }
        }
    }
    // that request is parsed on client side and updates list by adding new user
}

// we pass here disconnected user and send request to every socket to remove that user from online list
void remove_disconnected_user(client_t *disconnectedClient) {
    // prefix for client remove
    char removeUserStr[10000] = "+{RemoveDisconnectedUser}+ ";
    // information about client to be removed
    strcat(removeUserStr, disconnectedClient->name);
    // removing disconnected client
    for(int i = 0; i < MAX_CLIENTS; i++) {
        // if client exists
        if(clients[i]) {
            if(write(clients[i]->sockfd, removeUserStr, strlen(removeUserStr)) < 0) {
                printf("ERROR: write to descriptor failed\n");
                break;
            }
        }
    }
    // that request is parsed on client side and updates list by removing disconnected user
}

// function is sending message to specific client by his name passed as argument, its used for direct messages
void send_to_client_by_name(char* clientSourceName, char* clientTargetName, char* message) {
    // first thing is to find that client and his socket
    for(int i = 0; i < MAX_CLIENTS; i++) {
        // if client exists
        if(clients[i]) {
            // if name matches
            if(strcmp(clientTargetName, clients[i]->name) == 0) {
                printf("[+][MANHATTAN MESSAGES TRACKER]\n");
                printf("[MESSAGE SOURCE]:  %s\n", clientSourceName);
                printf("[MESSAGE TARGET]:  %s\n", clientTargetName);
                printf("[MESSAGE CONTENT]: %s\n", message);
                if(write(clients[i]->sockfd, message, strlen(message)) < 0) {
                    printf("ERROR: write to descriptor failed\n");
                    break;
                }
            }
        }
    }
}

// helping function, used to send direct message, that function retrives name of client that should receive sended message
void get_username_from_message(char* clientSourceName, char* message) {
    // pch is nickname of user [clientTarget]
    printf("%s\n", message);
    char *clientName;
    clientName = strtok(message, " ");
    message = strtok(NULL, "");
    send_to_client_by_name(clientSourceName, clientName, message);
}

// main function of direct messages, triggers rest of functions
void exchange_messages_between_clients(char* clientSourceName, char* message) {
    get_username_from_message(clientSourceName, message);
}


// very important function, receving file logic and writing it to folder of specific client
void write_file(int clientFd, char* clientName, char* fileName) {
        
        // first thing is size of file sended as first message from client to server
        char fileSize[1024];
        int bytesToReceive = recv(clientFd, fileSize, 1024, 0);

        if(bytesToReceive < 0 ) {
            printf("[-][ERROR]: No file size message!\n");
        }

        int remainingBytes = atoi(fileSize);

        // file creation and saving it in good folder
        FILE *fp;
        char *filename = fileName;
        char buffer[fileBufferSIZE];

        // go to the folder of specific client
        chdir(clientName);

        // create file there
        fp = fopen(filename, "ab");
        if(fp == NULL) {
            printf("[-][ERROR]: Error in creating file.");
            exit(1);
        }

        // reading file
        while(1) {

            int result = recv(clientFd, buffer, 1024, 0);


            if(result == -1) {
                printf("ERROR");
                break;
            }
            else if(result == 0) {
                printf("disconnected");
                break;
            }
            fwrite(buffer, 1, result, fp);
            printf("Received byte: %d\n", result);

            remainingBytes = remainingBytes - result;

            if(remainingBytes <= 0) {
                break;
                printf("RESULT %i", result);
            }

        }
        // back to main folder
        chdir("..");

        // all good
        printf("[+][FILE TRANSFER] File %s\n received with success!", fileName);
}


// this is the most important function of whole server side, every client has his thread and this function handles that client
void *handle_client(void *arg) {

    // buffer to receive messages
    char buffer[BUFFER_SIZE];
    char name[NAME_LEN];
    // flag to leave client loop
    int leave_flag = 0;
    clientsCount++;

    client_t *cli = (client_t*)arg;

    // first connection - we have to answer with current list of users to that
    if(recv(cli->sockfd, name, NAME_LEN, 0) <= 0 || strlen(name) < 0 || strlen(name) >= NAME_LEN - 1) {
        printf("[-][ERROR]: Connection without nickname!\n");
        leave_flag = 1;
    } else {
        // save name of connected client
        strcpy(cli->name, name);
        mkdir(cli->name, 0700);
        printf("[NEW CLIENT] %s has joined to the Manhattan Project!\n",cli->name);
        printf("[DIRECTORY CREATED] Directory for %s created. There will be stored files from this user.\n",cli->name);
        printf("[+] Number of connected users: %i\n", clientsCount);

        // if there is more than one client we return list of clients
        if(clientsCount > 1) {
            // get current users list of connected client
            current_users_list(cli);
            // update other clients users list 
            update_users_list_on_new_connection(cli);
        }
    }

    bzero(buffer, BUFFER_SIZE);

    // client loop - magic happens here
    while(1) {
        if(leave_flag) {
            break;
        }

        int receive = recv(cli->sockfd, buffer, BUFFER_SIZE, 0);

        // normal messsage
        // it will be always message sent from one client to another, so we need source and destination to forward message correctly
        // source is client who is sending message so we already have him, destination will be send in message as first word so we need to pull it out
        if(receive > 0) {
            if(strlen(buffer) > 0) {
                // check if its file transfer or message redirection request

                // file transfer
                char buffCopy[1024];
                strcpy(buffCopy, buffer);
                char* firstWord = strtok(buffCopy, " ");
                char* fileName = strtok(NULL, "");

                // file transfer request
                if(strcmp(firstWord,"+{FILETRANSFER}+") == 0) {
                    write_file(cli->sockfd, cli->name, fileName);
                }
                // message redirection request
                else {
                    exchange_messages_between_clients(cli->name, buffer);
                    str_trim_lf(buffer, strlen(buffer));
                }
                bzero(buffer, BUFFER_SIZE);
            }
        }
        // leaving the chat and user disconnection
        else if(receive == 0 || strcmp(buffer, "exit") == 0){
            clientsCount--;
            remove_disconnected_user(cli);
            printf("[CLIENT DISCONNECTED] %s has left Manhattan Project!\n",cli->name);
            printf("[-] Number of connected users: %i\n", clientsCount);
            if(clientsCount == 0) {
                printf("[ :( ] Server is empty.");
            }
            leave_flag = 1;
            bzero(buffer, BUFFER_SIZE); //
        }
        // errorr
        else {
            printf("[-][ERROR]: -1");
            leave_flag = 1;
        }
        bzero(buffer, BUFFER_SIZE);
    }
    close(cli->sockfd);
    // remove from queue
    queue_remove(cli->uid);
    free(cli);
    // delete thread used for that client
    pthread_detach(pthread_self());
    return NULL;
}

int main(int argc, char **argv) {

    // server settings
    char *ip = "192.168.226.135";
    int port = atoi(argv[1]);

    int option = 1;
    int listenFd = 0, connFd = 0;
    struct sockaddr_in serverAddr;
    struct sockaddr_in clientAddr;

    pthread_t tid;

    // socket settings
    listenFd = socket(AF_INET, SOCK_STREAM, 0);
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_addr.s_addr = inet_addr(ip);
    serverAddr.sin_port = htons(port);

    // signals
    // to get rid of bind error
    signal(SIGPIPE, SIG_IGN);

    if(setsockopt(listenFd, SOL_SOCKET, (SO_REUSEPORT | SO_REUSEADDR), (char*)&option, sizeof(option)) < 0) {
        printf("[-][ERROR]: setsockopt\n");
        return EXIT_FAILURE;
    }

    // binding
    if(bind(listenFd, (struct sockaddr*)&serverAddr, sizeof(serverAddr)) < 0) {
        printf("[-][ERROR]: bind\n");
        return EXIT_FAILURE;
    }

    // listen
    if(listen(listenFd, 10) < 0) {
        printf("[-][ERROR]: listen\n");
        return EXIT_FAILURE;
    }

    // manhattan project start 
    printf("[###] Project Manhattan Server is online [###]\n");

    // while loop to accept clients
    while(1) {
        socklen_t clientLen = sizeof(clientAddr);
        connFd = accept(listenFd, (struct sockaddr*)&clientAddr, &clientLen);

        // max clients
        if((clientsCount + 1) == MAX_CLIENTS) {
            printf("[-][MAX CLIENTS]: Maximum clients connected. Rejected client: ");
            printIpAddress(clientAddr);
            close(connFd);
            continue;
        }

        // client settings
        client_t *cli = (client_t *)malloc(sizeof(client_t));
        cli->address = clientAddr;
        cli->sockfd = connFd;
        cli->uid = uid++;

        // add client to queue
        queue_add(cli);

        // create thread for that client
        pthread_create(&tid, NULL, &handle_client, (void*)cli);

        // Reduce GPU Usage
        sleep(1);

    }

}