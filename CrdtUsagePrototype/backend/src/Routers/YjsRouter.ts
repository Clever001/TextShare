import { ClientSocket } from "../BaseServer/BaseServer";
import { YjsController } from "../Controllers/YjsController";
import { YjsDocumentKeeper } from "../Utils/YjsDocumentKeeper";
import { Router } from "./Router";

export class YjsRouter implements Router {
  private readonly clients: Set<ClientSocket>;
  private readonly document: YjsDocumentKeeper;

  public constructor(
    clients: Set<ClientSocket>,
    document: YjsDocumentKeeper
  ) {
    this.clients = clients;
    this.document = document;
  }

  public onConnect(currentClient: ClientSocket): void {
    const controller = new YjsController(this.clients, currentClient, this.document);
    controller.sendCurrentDocumentUpdate();
  }

  public onMessage(rawData: Buffer, currentClient: ClientSocket): void {
    const controller = new YjsController(this.clients, currentClient, this.document);
    controller.resendMessageToOtherClients(rawData);
  }
}