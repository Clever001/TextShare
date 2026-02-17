import { Logger } from "pino";
import WebSocket, { WebSocketServer } from "ws";
import { VersioningController } from "./VersioningController";
import { YjsEchoController } from "./YjsEchoController";
import { YjsDocument } from "../utils/YjsDocument";

interface ClientSocket extends WebSocket {
  isAlive: boolean;
}

interface BaseServerConfig {
  host: string,
  port: number,
  // yjsEchoStrategy: YjsEchoController,
  // versioningStrategy: VersioningController,
  logger: Logger,
}

export class BaseServer {
  private readonly logger: Logger;
  private readonly wss: WebSocketServer;
  private readonly clients: Set<ClientSocket>;

  // private readonly yjsEchoStrategy: YjsEchoController;
  // private readonly versioningStrategy: VersioningController;
  // private readonly yjsDocument: YjsDocument;

  public constructor(config: BaseServerConfig) {
    this.logger = config.logger.child({ name: BaseServer.name });
    this.wss = new WebSocketServer({
      host: config.host,
      port: config.port,
    });
    this.clients = new Set();
    // this.yjsEchoStrategy = config.yjsEchoStrategy;
    // this.versioningStrategy = config.versioningStrategy;

    this.setup();
  }

  private setup() {
    this.wss.on('connection', (socket: ClientSocket) => {
      this.logger.info('Новый клиент подключился');
      socket.isAlive = true;
    
      this.clients.add(socket);
    
      socket.on('message', (message: Buffer) => {
        try {
          // const decoded = Y.decode
          // this.logger.info('Получено сообщение:', decoded);
          
          if (isJson(message)) {
            this.logger.info("Новое сообщение в виде json");
            const parsedMessage = JSON.parse(message.toString('utf-8'));
            const strMessage = JSON.stringify(parsedMessage);
            this.clients.forEach((client) => {
              if (client !== socket && client.readyState === WebSocket.OPEN) {
                client.send(strMessage);
              }
            })

          } else {
            this.logger.info("Новое сообщение в бинарном виде");
            this.clients.forEach((client) => {
              if (client !== socket && client.readyState === WebSocket.OPEN) {
                client.send(message);
              }
            });
          }

        } catch (error) {
          console.error('Ошибка обработки сообщения:', error);
        }
      });
    
      socket.on('close', () => {
        this.logger.info('Клиент отключился');
        this.clients.delete(socket);
      });
    
      socket.on('error', (err) => {
        this.logger.error({
          info: 'Ошибка в WebSocket соединении',
          error: err,
        });
      });

      socket.on('pong', () => {
        socket.isAlive = true;
      });
    });

    this.wss.on('listening', () => {
      this.logger.info('Сервер запущен на ws://192.168.0.227:1234');
    });

    setInterval(() => {
      this.clients.forEach((socket) => {
        if (!socket.isAlive) return socket.terminate();

        socket.isAlive = false;
        socket.ping();
      });
    }, 30000);
  }
}

function isJson(data: Buffer): boolean {
  try {
    JSON.parse(data.toString('utf-8'));
    return true;
  } catch (error) {
    if (error instanceof SyntaxError) {
      return false;
    }
    throw error;
  }
}
