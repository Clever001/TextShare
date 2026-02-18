import pino from "pino";
import { ClientSocket } from "../BaseServer";
import { NewMessage } from "../dtos";
import { BaseController } from "./BaseContoller";

export class MessageController extends BaseController {
  private readonly logger = pino({name: MessageController.name});

  public constructor(
    clients: Set<ClientSocket>,
    currentClient: ClientSocket
  ) {
    super(clients, currentClient);
  }

  public processNewMessage(dto: NewMessage): void {
    const dtoStr = JSON.stringify(dto);
    this.clients.forEach(client => {
      if (client !== this.currentClient) {
        client.send(dtoStr);
        this.logger.info("Sended json: " + dtoStr);
      }
    })
  }
}