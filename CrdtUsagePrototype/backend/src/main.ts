import WebSocket, { WebSocketServer } from 'ws';
import * as Y from 'yjs';
import pino, { Logger } from 'pino';
import { BaseServer, ClientSocket } from './BaseServer';
import { MessagesRouter } from './Routers/MessagesRouter';
import { MessageType, NewMessage } from './dtos';
import { MessageController } from './Controllers/MessageController';
import { BaseServerBuilder } from './BaseServerBuilder';

const yjsServer = new BaseServerBuilder()
  .withHostAndPort('192.168.0.227', 1234)
  .withEmptyClientsList()
  .withYjsRouter()
  .withLoggerName('yjsServer')
  .build();

const massagesServer = new BaseServerBuilder()
  .withHostAndPort('192.168.0.227', 1235)
  .withEmptyClientsList()
  .withMessagesRouter()
  .withLoggerName('massagesServer')
  .build();
