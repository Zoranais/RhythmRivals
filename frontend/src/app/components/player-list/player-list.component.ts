import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Player } from '../../models/player';
import { CommonModule } from '@angular/common';
import { FadeOutScoreComponent } from '../fade-out-score/fade-out-score.component';

@Component({
  selector: 'app-player-list',
  standalone: true,
  imports: [CommonModule, FadeOutScoreComponent],
  templateUrl: './player-list.component.html',
  styleUrl: './player-list.component.sass',
})
export class PlayerListComponent implements OnChanges {
  @Input() players: Player[] = [];
  private oldPlayers: Player[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    this.oldPlayers = changes['players'].previousValue ?? [];
    this.players = changes['players'].currentValue.sort(
      (a, b) => b.score - a.score
    );
  }

  public getScoreDifference(player: string) {
    const newPlayer = this.players.find((x) => x.name == player);
    const oldPlayer = this.oldPlayers.find((x) => x.name == player);

    if (oldPlayer == undefined) {
      return newPlayer.score;
    } else {
      return newPlayer.score - oldPlayer.score;
    }
  }
}
