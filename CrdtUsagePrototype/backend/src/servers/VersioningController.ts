import { Logger } from "pino";
import { WebSocketServer } from "ws";

interface VersioningServerConfig {
  host: string;
  port: number;
  logger: Logger;
}

export class VersioningController {
  private readonly logger: Logger;
  private readonly wss: WebSocketServer;
  private readonly clients: Set<WebSocket>;

  constructor(config: VersioningServerConfig) {
    this.logger = config.logger;
    this.wss = new WebSocketServer({
      host: config.host,
      port: config.port,
    });
    this.clients = new Set();
  }

  private setup() {
    this.wss.on('connection', (socket: WebSocket) => {
      
    })
  }
}