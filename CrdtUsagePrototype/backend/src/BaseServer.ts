import pino, { Logger } from "pino";
import WebSocket, { WebSocketServer } from "ws";
import { Router } from "./Routers/Router";

export interface ClientSocket extends WebSocket {
  isAlive: boolean
}

export interface BaseServerConfig {
  host: string,
  port: number,
  clients: Set<ClientSocket>,
  router: Router,
  loggerName: string
}

export class BaseServer {
  private readonly logger: Logger;
  private readonly wss: WebSocketServer;
  private readonly clients: Set<ClientSocket>;
  private readonly router: Router;

  public constructor(config: BaseServerConfig) {
    // this.logger = config.logger.child({ name: BaseServer.name });
    this.logger = pino( {name: config.loggerName});
    this.wss = new WebSocketServer({
      host: config.host,
      port: config.port,
    });
    this.clients = config.clients;
    this.router = config.router;

    this.setup();
  }

  private setup() {
    this.wss.on('connection', (socket: ClientSocket) => {
      this.logger.info('Новый клиент подключился');
      socket.isAlive = true;
    
      this.clients.add(socket);
    
      socket.on('message', (message: Buffer) => {
        this.logger.info("Получено сообщение.");

        try {
          this.router.route(message, socket);
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
      let addressStr;
      const address = this.wss.address();
      if (address && typeof address !== 'string') {
        const addrInfo = address as WebSocket.AddressInfo;
        addressStr = `ws:${addrInfo.address}:${addrInfo.port}`;
      } else if (typeof address == 'string') {
        addressStr = addressStr;
      } else {
        addressStr = 'Unknown';
      }
      this.logger.info(`Сервер запущен на ${addressStr}.`);
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
