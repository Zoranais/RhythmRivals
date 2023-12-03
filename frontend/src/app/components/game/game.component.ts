import { Component, OnInit } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { Game } from '../../models/game';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-game',
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatInputModule,
    MatButtonModule,
    FormsModule,
  ],
  templateUrl: './game.component.html',
  styleUrl: './game.component.sass',
})
export class GameComponent implements OnInit {
  public isLoading = true;
  public gameId: string;
  public isJoined = false;
  public name = '';
  public game: Game | undefined = undefined;

  private connection: signalR.HubConnection;

  constructor(private router: Router, private route: ActivatedRoute) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/GameHub`, {})
      .build();

    this.gameId = route.snapshot.params['gameId'];
  }

  ngOnInit(): void {
    this.connection
      .start()
      .then(() => (this.isLoading = false))
      .catch((err) => {
        console.log('Error while starting connection: ' + err);
        this.router.navigate(['']);
      });
  }

  public validateName() {
    return this.name.length >= 3;
  }

  public join() {
    this.connection
      .invoke('Join', this.gameId, this.name)
      .then(() => console.log('Join Executed'))
      .catch((err) => {
        console.log('Error while invoking: ' + err);
      });
  }
}
