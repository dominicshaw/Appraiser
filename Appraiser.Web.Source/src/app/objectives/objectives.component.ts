import { Component, OnInit, Input } from '@angular/core';
import { ObjectiveDTO } from '../generated-services';

@Component({
  selector: 'app-objectives',
  templateUrl: './objectives.component.html',
  styleUrls: ['./objectives.component.css']
})
export class ObjectivesComponent implements OnInit {

  @Input() objectives: ObjectiveDTO[];
  @Input() title: string;
  @Input() description: string;

  expand: boolean;

  constructor() {
    this.title = 'Objectives';
    this.description = '';
    this.expand = true;
  }

  ngOnInit() {
  }

  addObjective(): void {

    var dto = new ObjectiveDTO();

    dto.shortName = 'Objective ' + (this.objectives.length + 1);
    dto.description = '';
    this.objectives.push(dto);

    this.expand = true;

  }

}
