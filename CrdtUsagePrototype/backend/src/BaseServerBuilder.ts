import { Logger } from "pino";
import { BaseServer, BaseServerConfig, ClientSocket } from "./BaseServer";
import { MessagesRouter } from "./Routers/MessagesRouter";
import * as DTOs from "./dtos";
import { MessageController } from "./Controllers/MessageController";
import { YjsRouter } from "./Routers/YjsRouter";
import { Router } from "./Routers/Router";

export class BaseServerBuilder {
  private host: string | null = null;
  private port: number | null = null;
  private clients: Set<ClientSocket> | null = null;
  private router: Router | null = null;
  private loggerName: string | null = null;

  public withHostAndPort(host: string, port: number) : BaseServerBuilder {
    this.host = host;
    this.port = port;
    return this;
  }

  public withEmptyClientsList() : BaseServerBuilder {
    this.clients = new Set<ClientSocket>();
    return this;
  }

  public withMessagesRouter(): BaseServerBuilder {
    if (!this.clients) {
      throw new Error('Cannot define router without access to clients list');
    }

    const clients = this.clients!;

    const messagesRouter = new MessagesRouter();
    messagesRouter.on(
      DTOs.MessageType.NewMessage,
      (dto, currentClient) => {
        const messageController = new MessageController(clients!, currentClient);
        messageController.processNewMessage(dto as DTOs.NewMessage);
      }
    );

    this.router = messagesRouter;

    return this;
  }

  public withYjsRouter(): BaseServerBuilder {
    if (!this.clients) {
      throw new Error('Cannot define router without access to clients list');
    }
    this.router = new YjsRouter(this.clients!);
    return this;
  }

  public withLoggerName(name: string) : BaseServerBuilder {
    this.loggerName = name;
    return this;
  }


  public build(): BaseServer {
    this.throwIfUninit(this.host, 'host');
    this.throwIfUninit(this.port, 'port');
    this.throwIfUninit(this.clients, 'clients');
    this.throwIfUninit(this.router, 'router');
    this.throwIfUninit(this.loggerName, 'loggerName');

    return new BaseServer({
      host: this.host!,
      port: this.port!,
      clients: this.clients!,
      router: this.router!,
      loggerName: this.loggerName!
    })
  }

  private throwIfUninit<T>(param: T, paramName: string) {
    if (!param) {
      throw new Error("Parameter is uninitialized: " + paramName);
    }
  }
}