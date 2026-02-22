import pino from "pino";
import { ClientSocket } from "../BaseServer/BaseServer";
import { MessageDto, MessageType } from "../Utils/Dtos"
import { Router } from "./Router";

export type MessageHandler = (dto: MessageDto, currentClient: ClientSocket) => void;

export class VersionRouter implements Router {
  private readonly logger = pino({name: VersionRouter.name});
  private readonly handlers = new Map<MessageType, MessageHandler>();
  private onConnectHandler: ((currentClient: ClientSocket)=> void) | null = null;
  
  public setOnMessage(
    messageType: MessageType,
    handler: MessageHandler
  ): void {
    this.handlers.set(messageType, handler);
  }

  public setOnConnect(handler: (currentClient: ClientSocket)=> void): void {
    this.onConnectHandler = handler;
  }

  public onConnect(currentClient: ClientSocket): void {
    if (this.onConnectHandler) {
      this.onConnectHandler(currentClient);
    }
  }

  public onMessage(rawData: Buffer, currentClient: ClientSocket): void {
    let data: MessageDto;

    try {
      const parsedData = JSON.parse(rawData.toString("utf-8"));
      if ('type' in parsedData) {
        data = parsedData as MessageDto;
      } else {
        this.throwInvalidMessage();
        return;
      }
    } catch (e) {
      if (e instanceof SyntaxError) {
        this.throwInvalidMessage();
        return;
      } else {
        throw e;
      }
    }

    this.logger.info("Got data type: " + data.type);
    const handler = this.handlers.get(data.type);
    
    if (!handler) {
      const errorMessage = `Unknown message type: ${data.type}`;
      this.logger.error(errorMessage);
      throw new Error(errorMessage);
    }

    handler(data, currentClient);
  }

  private throwInvalidMessage(): void {
    throw new Error(`Invalid message content.`);
  }
}