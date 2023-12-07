import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { CommonModule } from '@angular/common';
import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';

@Component({
  selector: 'app-fade-out-score',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './fade-out-score.component.html',
  styleUrl: './fade-out-score.component.sass',
  animations: [
    trigger('fade', [
      state('visible', style({ opacity: 1 })),
      state('invisible', style({ opacity: 0 })),
      transition('visible => invisible', [animate('2s')]),
      transition('invisible => visible', [animate('0.5s')]),
      transition(':enter', [
        style({ opacity: 0 }),
        animate('0.5s', style({ opacity: 1 })),
      ]),
    ]),
  ],
})
export class FadeOutScoreComponent implements OnChanges, OnInit {
  @Input() scoreDifference: number | undefined;
  public isVisible = false;

  ngOnInit(): void {
    this.isVisible = true;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['scoreDifference']) {
      this.isVisible = true;
      setTimeout(() => {
        this.isVisible = false;
      }, 2000);
    }
  }
}
