import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VolumeService {
  private volume: number = 0.2;
  private volumeKey = 'volume';
  private onVolumeChanged = new Subject<number>();

  public volumeChangedEvent$ = this.onVolumeChanged.asObservable();

  constructor() {
    var savedVolume = localStorage.getItem(this.volumeKey);
    if (savedVolume != null) {
      this.volume = Number.parseFloat(savedVolume);
    } else {
      this.setVolume(this.volume);
    }
  }

  public getVolume() {
    return this.volume;
  }

  public setVolume(value: number) {
    this.volume = value;
    localStorage.setItem(this.volumeKey, this.volume.toString());

    this.onVolumeChanged.next(value);
  }
}
