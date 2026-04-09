import pino from "pino";
import { ClientSocket } from "../BaseServer/BaseServer";
import { YjsDocumentKeeper } from "../Utils/YjsDocumentKeeper";
import { BaseController } from "./BaseContoller";

export class YjsController extends BaseController {
  private readonly document: YjsDocumentKeeper;
  private readonly logger = pino({name: YjsController.name});

  public constructor (
    clients: Set<ClientSocket>,
    currentClient: ClientSocket,
    document: YjsDocumentKeeper,
  ) {
    super(clients, currentClient);
    this.document = document;
  }

  public sendCurrentDocumentUpdate() {
    if (this.clients.size === 1) {
      this.logger.info("Было выслано начальное состояние документа.");
      this.currentClient.send(this.document.getDocumentStateUpdateMessage());
    }
  }

  // public resendMessageToOtherClients(rawData: Buffer) {
  //   const nullableResponse = this.document.updateDocument(rawData);
  //   if (nullableResponse) {
  //     this.currentClient.send(nullableResponse);
  //   }

  //   this.clients.forEach(client => {
  //     if (client.isAlive && client !== this.currentClient) {
  //       client.send(rawData);
  //     }
  //   })
  // }

  public resendMessageToOtherClients(rawData: Buffer) {
    const { response, shouldBroadcast } = this.document.updateDocument(rawData);
    if (response) {
      // this.logger.info("Было выслано изменение с сервера");
      this.currentClient.send(response);
    }

    if (shouldBroadcast) {
      // this.logger.info("Было отправлено изменение другим пользователям.");
      this.clients.forEach(client => {
        if (client.isAlive && client !== this.currentClient) {
          client.send(rawData);
        }
      })
    }
  }
}