import { ClientSocket } from "../BaseServer";
import { YjsController } from "../Controllers/YjsController";
import { Router } from "./Router";

export class YjsRouter implements Router {
  private readonly clients: Set<ClientSocket>;

  public constructor(clients: Set<ClientSocket>) {
    this.clients = clients;
  }

  public route(rawData: Buffer, currentClient: ClientSocket): void {
    const controller = new YjsController(this.clients, currentClient);
    controller.resendMessageToOtherClients(rawData);
  }
}