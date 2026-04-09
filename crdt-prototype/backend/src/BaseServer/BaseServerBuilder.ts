import { BaseServer, ClientSocket } from "./BaseServer";
import { VersionRouter } from "../Routers/VersionRouter";
import * as DTOs from "../Utils/Dtos";
import { VersionController } from "../Controllers/VersionController";
import { YjsRouter } from "../Routers/YjsRouter";
import { Router } from "../Routers/Router";
import { YjsDocumentKeeper } from "../Utils/YjsDocumentKeeper";
import { VersionsKeeper } from "../Utils/VersionsKeeper";

export class BaseServerBuilder {
  private host: string | null = null;
  private port: number | null = null;
  private clients: Set<ClientSocket> | null = null;
  private documentKeeper: YjsDocumentKeeper | null = null;
  private router: Router | null = null;
  private loggerName: string | null = null;
  private versionsKeeper: VersionsKeeper | null = null;

  public withHostAndPort(host: string, port: number) : BaseServerBuilder {
    this.host = host;
    this.port = port;
    this.clients = new Set<ClientSocket>();
    return this;
  }

  public withEmptyDocument(): BaseServerBuilder {
    this.documentKeeper = YjsDocumentKeeper.createEmptyDocument();

    return this;
  }

  public withDocument(keeper: YjsDocumentKeeper) {
    this.documentKeeper = keeper;
    return this;
  }

  public withVersionsKeeper(keeper: VersionsKeeper) {
    this.versionsKeeper = keeper;
    return this;
  }

  public withVersionsRouter(): BaseServerBuilder {
    this.throwIfUninit(this.clients, 'clients');
    this.throwIfUninit(this.documentKeeper, 'documentKeeper');
    this.throwIfUninit(this.versionsKeeper, 'versionsKeeper');

    const messagesRouter = new VersionRouter();
    messagesRouter.setOnConnect(
      (currentClient) => {
        const versionController = new VersionController(
          this.clients!, 
          currentClient, 
          this.documentKeeper!,
          this.versionsKeeper!,
        );
        versionController.sendVersionsList();
      }
    );
    messagesRouter.setOnMessage(
      "createNewVersion",
      (dto, currentClient) => {
        const messageController = new VersionController(
          this.clients!, 
          currentClient, 
          this.documentKeeper!,
          this.versionsKeeper!,
        );
        messageController.createNewVersion(dto as DTOs.VersionDto);
      }
    );
    messagesRouter.setOnMessage(
      "sendCertainVersion",
      (dto, currentClient) => {
        const controller = new VersionController(
          this.clients!,
          currentClient,
          this.documentKeeper!,
          this.versionsKeeper!,
        );
        controller.sendCertainVersion(dto as DTOs.SendVersionRequest);
      }
    );
    messagesRouter.setOnMessage(
      "deleteCertainVersion",
      (dto, currentClient) => {
        const controller = new VersionController(
          this.clients!,
          currentClient,
          this.documentKeeper!,
          this.versionsKeeper!,
        );
        controller.deleteCertainVersion(dto as DTOs.DeleteVersionRequest);
      }
    );
    messagesRouter.setOnMessage(
      "renameCertainVersion",
      (dto, currentClient) => {
        const controller = new VersionController(
          this.clients!,
          currentClient,
          this.documentKeeper!,
          this.versionsKeeper!,
        );
        controller.renameCertainVersion(dto as DTOs.RenameVersionRequest);
      }
    );
    messagesRouter.setOnMessage(
      "rollbackToVersion",
      (dto, currentClient) => {
        const controller = new VersionController(
          this.clients!,
          currentClient,
          this.documentKeeper!,
          this.versionsKeeper!,
        );
        controller.rollbackToVersion(dto as DTOs.RollbackDocRequest);
      }
    );

    this.router = messagesRouter;

    return this;
  }

  public withYjsRouter(): BaseServerBuilder {
    this.throwIfUninit(this.clients, 'clients');
    this.throwIfUninit(this.documentKeeper, 'documentKeeper');
    
    this.router = new YjsRouter(this.clients!, this.documentKeeper!);
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
      throw new Error("Parameter is uninitialized: " + paramName + ".");
    }
  }
}