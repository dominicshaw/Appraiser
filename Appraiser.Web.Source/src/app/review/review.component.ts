import { Component, OnInit, Input } from '@angular/core';
import { ReviewDTO } from '../generated-services';

@Component({
  selector: 'app-review',
  templateUrl: './review.component.html',
  styleUrls: ['./review.component.css']
})
export class ReviewComponent implements OnInit {

  @Input() review: ReviewDTO;
  @Input() title: string;
  @Input() description: string;

  constructor() { 
    this.title = 'Reviews';
    this.description = '';
  }

  ngOnInit() {
  }

}
