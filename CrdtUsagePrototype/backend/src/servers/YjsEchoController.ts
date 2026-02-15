import { Logger } from "pino";
import WebSocket, { WebSocketServer } from "ws";
import { YjsDocument } from "../utils/YjsDocument";

interface ClientSocket extends WebSocket {
  isAlive: boolean;
}

interface YjsEchoStrategyConfig {
  logger: Logger;
}

export class YjsEchoController {}

// export class YjsEchoController {
//   private readonly logger: Logger;
//   private readonly yjsDocument: YjsDocument;
//   private readonly clients: Set<ClientSocket>;
  
//   public constructor(config: YjsEchoStrategyConfig) {
    
//   }

//   public onMessage(message: Buffer): void {
//     this.clients.forEach((client) => {
//       if (client !== )
//     })
//   }
// }