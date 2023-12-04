import { Injectable, OnDestroy } from '@angular/core';
import { VolumeService } from './volume.service';
import { Subject, takeUntil } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class AudioService implements OnDestroy {
  private audio = new Audio();
  private $unsubscribe = new Subject<void>();

  constructor(private volumeService: VolumeService) {
    volumeService.volumeChangedEvent$
      .pipe(takeUntil(this.$unsubscribe))
      .subscribe((volume) => (this.audio.volume = volume));
  }

  public play(src: string) {
    this.audio.src = src;
    this.audio.load();
    this.audio.volume = this.volumeService.getVolume();
    this.audio.play();
  }

  public stop() {
    this.audio.pause();
    this.audio.currentTime = 0;
  }

  ngOnDestroy(): void {
    this.$unsubscribe.next();
    this.$unsubscribe.complete();
  }
}
