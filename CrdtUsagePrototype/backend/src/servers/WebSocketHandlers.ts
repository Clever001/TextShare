export interface WebSocketHandler {
  canHandle(message: Buffer): boolean;
  onMessage(message: Buffer): void;
}

export class WebSocketHandlerFactory {
  private readonly handlers: WebSocketHandler[] = [];

  
}