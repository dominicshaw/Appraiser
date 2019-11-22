import { Component, OnInit, Input } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { StaffService } from '../generated-services';

import { StaffDTO } from '../generated-services';

@Component({
  selector: 'app-staff-detail',
  templateUrl: './staff-detail.component.html',
  styleUrls: ['./staff-detail.component.css']
})

export class StaffDetailComponent implements OnInit {

  @Input() staff: StaffDTO;

  constructor(
    private route: ActivatedRoute,
    private staffService: StaffService,
    private location: Location
  ) {

  }

  ngOnInit(): void {
    this.getStaff();
  }

  getStaff(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.staffService.getStaff(id)
      .subscribe(s => this.staff = s);
  }

}
