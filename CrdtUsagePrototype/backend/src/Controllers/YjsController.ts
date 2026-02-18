import { ClientSocket } from "../BaseServer";
import { BaseController } from "./BaseContoller";

export class YjsController extends BaseController {
  public constructor (
    clients: Set<ClientSocket>,
    currentClient: ClientSocket
  ) {
    super(clients, currentClient);
  }

  public resendMessageToOtherClients(rawData: Buffer) {
    this.clients.forEach(client => {
      if (client.isAlive && client !== this.currentClient) {
        client.send(rawData);
      }
    })
  }
}