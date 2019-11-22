import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ObjectiveDTO } from '../generated-services';
import { AgInputTextArea } from '@ag-grid-community/core';

import { ObjectivesService } from '../generated-services';
import { MatCheckboxChange } from '@angular/material/checkbox';

@Component({
  selector: 'app-objective',
  templateUrl: './objective.component.html',
  styleUrls: ['./objective.component.css']
})
export class ObjectiveComponent implements OnInit {

  @ViewChild('txtMeasurement', { static: false }) txtMeasurement: AgInputTextArea;

  @Input() objectives: ObjectiveDTO[];
  @Input() objective: ObjectiveDTO;
  @Input() title: string;
  @Input() description: string;

  objectiveAchieved: boolean;

  constructor(private service: ObjectivesService) {
  }

  ngOnInit() {
    this.title = this.objective.shortName.substr(0, 20);
    this.description = this.objective.description.substr(0, 30);

    this.objectiveAchieved = this.objective.achieved != null && this.objective.achieved == 100;
  }

  toggleAchieved(args: MatCheckboxChange): void {
    if (!args.checked) {
      this.objective.achieved = 0;
    } else {
      this.objective.achieved = 100;
    }
  }

  deleteObjective(): void {
    if (confirm('Are you sure you want to delete this objective? This cannot be un-done.')) {
      if (this.objective.id !== 0) {
        this.service.deleteObjective(this.objective.id)
          .subscribe(o => console.log('deleted objective ' + o.id));
      }

      for (var i = 0; i < this.objectives.length; i++) {
        if (this.objectives[i] === this.objective) {
          this.objectives.splice(i, 1);
        }
      }
    }
  }

}
