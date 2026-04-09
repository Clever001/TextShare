import { ClientSocket } from "../BaseServer/BaseServer";

export interface Router {
  onConnect(currentClient: ClientSocket): void;
  onMessage(rawData: Buffer, currentClient: ClientSocket): void;
}