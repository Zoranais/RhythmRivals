import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatSliderModule } from '@angular/material/slider';
import { VolumeService } from '../../services/volume.service';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { environment } from '../../../environments/environment';
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatButtonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    MatSliderModule,
    FormsModule,
    MatIconModule,
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.sass',
})
export class HeaderComponent {
  public volume: number;
  constructor(private volumeService: VolumeService) {
    this.volume = this.volumeService.getVolume();
    console.log(environment.apiUrl);
  }
  public setVolume() {
    console.log(this.volume);
    return this.volumeService.setVolume(this.volume);
  }
}
