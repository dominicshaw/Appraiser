import { Component, OnInit, Input } from '@angular/core';
import { TrainingDTO, ReviewDTO } from '../generated-services';

import { TrainingService } from '../generated-services';

@Component({
  selector: 'app-training',
  templateUrl: './training.component.html',
  styleUrls: ['./training.component.css']
})
export class TrainingComponent implements OnInit {

  @Input() review: ReviewDTO;
  @Input() training: TrainingDTO;
  @Input() title: string;
  @Input() description: string;

  constructor(private service: TrainingService) {
    this.title = 'Training';
    this.description = '';
  }

  ngOnInit() {
    this.title = this.training.name.substr(0, 20);
  }

  deleteObjective(): void {

    if (confirm('Are you sure you want to delete this objective? This cannot be un-done.')) {

      if (this.training.id !== 0)
        this.service.deleteTraining(this.training.id)
          .subscribe(o => console.log('deleted objective ' + o.id));

      for (var i = 0; i < this.review.training.length; i++) {
        if (this.review.training[i] === this.training) {
          this.review.training.splice(i, 1);
        }
      }

    }

  }
}
