import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { Round } from '../../models/round';
import { CommonModule } from '@angular/common';
import { AudioService } from '../../services/audio.service';

@Component({
  selector: 'app-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './question.component.html',
  styleUrl: './question.component.sass',
})
export class QuestionComponent implements OnChanges {
  @Input() round: Round;
  @Output() answerSubmitted = new EventEmitter<string>();

  constructor(private audioService: AudioService) {}

  ngOnChanges(simpleChanges: SimpleChanges) {
    if (simpleChanges['round'] && this.round) {
      this.audioService.play(this.round.previewUrl);
    }
  }

  public submitAnswer(answer: string) {
    this.answerSubmitted.emit(answer);
  }
}
