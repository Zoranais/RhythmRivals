import { GameState } from './game-state';
import { Player } from './player';

export interface Game {
  id: string;
  name: string;
  roundCount: number;
  players: Player[];
  state: GameState;
}
