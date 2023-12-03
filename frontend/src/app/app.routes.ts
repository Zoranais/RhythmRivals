import { Routes } from '@angular/router';
import { CreateGameComponent } from './components/create-game/create-game.component';
import { JoinGameComponent } from './components/join-game/join-game.component';
import { GameComponent } from './components/game/game.component';

export const routes: Routes = [
  { path: 'create-game', component: CreateGameComponent },
  { path: 'join-game', component: JoinGameComponent },
  { path: 'game/:gameId', component: GameComponent },
  { path: '', redirectTo: 'join-game', pathMatch: 'full' },
];
