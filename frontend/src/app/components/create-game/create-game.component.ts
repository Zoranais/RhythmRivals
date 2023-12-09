import { Component } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Form, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateGame } from '../../models/create-game';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Subject, takeUntil } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-game',
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  templateUrl: './create-game.component.html',
  styleUrl: './create-game.component.sass',
})
export class CreateGameComponent {
  public form: FormGroup;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private httpClient: HttpClient,
    private router: Router
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      spotifyUrl: ['', Validators.required],
      rounds: [3, [Validators.required, Validators.min(3), Validators.max(36)]],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const formData = this.form.value;
      const dto = {
        name: formData.name,
        spotifyUrl: formData.spotifyUrl,
        roundCount: formData.rounds,
      } as CreateGame;

      console.log(dto);
      var response = this.httpClient.post(
        `${environment.apiUrl}/api/game`,
        dto
      );
      response.pipe(takeUntil(this.unsubscribe$)).subscribe((gameId) => {
        this.router.navigate([`/game/${gameId}`]);
      });
    }
  }
}
