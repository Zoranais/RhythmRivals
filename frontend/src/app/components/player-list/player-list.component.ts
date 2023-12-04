import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Player } from '../../models/player';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-player-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './player-list.component.html',
  styleUrl: './player-list.component.sass',
})
export class PlayerListComponent implements OnChanges {
  @Input() players: Player[] = [];

  ngOnChanges(changes: SimpleChanges): void {
    this.players = changes['players'].currentValue.sort(
      (a, b) => b.score - a.score
    );
  }
}
