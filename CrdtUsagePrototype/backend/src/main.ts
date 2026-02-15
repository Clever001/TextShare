import WebSocket, { WebSocketServer } from 'ws';
import * as Y from 'yjs';
import pino, { Logger } from 'pino';
import { BaseServer } from './servers/BaseServer';

const logger: Logger = pino({
  name: 'Main',
  level: 'info',
});

const yjsEchoServer = new BaseServer({
  host: '192.168.0.227',
  port: 1234,
  logger: logger,
})

