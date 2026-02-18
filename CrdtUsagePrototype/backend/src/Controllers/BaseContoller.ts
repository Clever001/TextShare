import { ClientSocket } from "../BaseServer";

export class BaseController {
  protected readonly clients: Set<ClientSocket>;
  protected readonly currentClient: ClientSocket;

  protected constructor (
    clients: Set<ClientSocket>,
    currentClient: ClientSocket
  ) {
    this.clients = clients;
    this.currentClient = currentClient;
  }
}