import pino from "pino";
import { ClientSocket } from "../BaseServer/BaseServer";
import { BaseController } from "./BaseContoller";
import { YjsDocumentKeeper } from "../Utils/YjsDocumentKeeper";
import { VersionsKeeper } from "../Utils/VersionsKeeper";
import { DeleteVersionRequest, MessageType, RenamedVersionRequest, RenameVersionRequest, SendVersionRequest, VersionDto, VersionsListDto, VersionState } from "../Utils/Dtos";

export class VersionController extends BaseController {
  private readonly logger = pino({ name: VersionController.name });
  private readonly documentKeeper: YjsDocumentKeeper;
  private readonly versionsKeeper: VersionsKeeper;

  public constructor(
    clients: Set<ClientSocket>,
    currentClient: ClientSocket,
    documentKeeper: YjsDocumentKeeper,
    versionsKeeper: VersionsKeeper,
  ) {
    super(clients, currentClient);
    this.documentKeeper = documentKeeper;
    this.versionsKeeper = versionsKeeper;
  }

  public sendVersionsList() {
    const versions = this.versionsKeeper.getAllVersions();
    const dto: VersionsListDto = {
      type: "versionsList",
      versions: versions
    };
    const dtoStr = JSON.stringify(dto);
    this.currentClient.send(dtoStr);
  }

  public createNewVersion(dto: VersionDto): void {
    this.versionsKeeper.createNewVersion(dto, this.documentKeeper);
    dto.type = "newVersionCreated";
    const dtoStr = JSON.stringify(dto);
    this.clients.forEach(client => {
      if (client.isAlive) {
        client.send(dtoStr);
      }
    });
    this.logger.info("CreatedNewVersion");
  }

  public sendCertainVersion(dto: SendVersionRequest): void {
    const nullableVersion = this.versionsKeeper.getVersion(dto.versionId);
    if (nullableVersion) {
      const sendDto: VersionState = {
        type: "sendedCertainVersion",
        versionState: this.binaryToString(nullableVersion.documentState)
      };
      const sendDtoStr = JSON.stringify(sendDto);
      this.currentClient.send(sendDtoStr);
    }
  }

  public deleteCertainVersion(dto: DeleteVersionRequest): void {
    const deleted = this.versionsKeeper.deleteVersion(dto.versionId);
    if (deleted) {
      dto.type = "deletedCertainVersion";
      const dtoStr = JSON.stringify(dto);
      this.clients.forEach(client => {
        if (client.isAlive) {
          client.send(dtoStr);
        }
      });
    }
  }

  public renameCertainVersion(dto: RenameVersionRequest): void {
    const versionToDocument = this.versionsKeeper.getVersion(dto.versionId);
    if (!versionToDocument) return;
    const renamed = this.versionsKeeper.updateVersionName(versionToDocument.version.id, dto.newVersionName);
    if (renamed) {
      const renamedDto: RenamedVersionRequest = {
        type: 'renamedCertainVersion',
        id: versionToDocument.version.id,
        name: versionToDocument.version.name,
        createdTime: versionToDocument.version.createdTime,
      }
      const renamedDtoStr = JSON.stringify(renamedDto);
      this.clients.forEach(client => {
        if (client.isAlive) {
          client.send(renamedDtoStr);
        }
      })
    }
  }

  private binaryToString(rawData: Uint8Array): string {
    const charCodes: number[] = Array.from(rawData);
    const binaryString = String.fromCharCode.apply(null, charCodes);
    return btoa(binaryString);
  }
}