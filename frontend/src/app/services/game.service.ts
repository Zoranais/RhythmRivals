import { HttpClientModule, HttpClient } from '@angular/common/http';
import { Injectable, NgModule, OnDestroy } from '@angular/core';
import { environment } from '../../environments/environment';
import { takeUntil, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GameService implements OnDestroy {
  private unsubscribe$ = new Subject<void>();

  constructor(private httpClient: HttpClient) {}

  public isExist(gameId: string) {
    let subject = new Subject<boolean>();
    this.httpClient
      .get(`${environment.apiUrl}/api/game/isExist/${gameId}`)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((value) => {
        subject.next((value as boolean) ?? false);
      });

    return subject.asObservable();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
