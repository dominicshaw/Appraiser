import { Component, OnInit, Input } from '@angular/core';
import { TrainingDTO, ReviewDTO } from '../generated-services';

@Component({
  selector: 'app-training-list',
  templateUrl: './training-list.component.html',
  styleUrls: ['./training-list.component.css']
})
export class TrainingListComponent implements OnInit {

  @Input() review: ReviewDTO;
  @Input() title: string;
  @Input() description: string;

  expand: boolean;

  constructor() {
    this.title = 'Training';
    this.description = '';
    this.expand = true;
  }

  ngOnInit() {
  }

  addTraining(): void {

    var dto = new TrainingDTO();

    dto.name = 'New Training'
    dto.reviewId = this.review.id;

    this.review.training.push(dto);

    this.expand = true;

  }

}
