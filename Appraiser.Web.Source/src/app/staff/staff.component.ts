import { Component, OnInit } from '@angular/core';
import { StaffDTO } from '../generated-services';
import { StaffService } from '../generated-services';

@Component({
  selector: 'app-staff',
  templateUrl: './staff.component.html',
  styleUrls: ['./staff.component.css']
})

export class StaffComponent implements OnInit {

  staffList: StaffDTO[];
  selectedStaff: StaffDTO;

  constructor(private staffService: StaffService) { }

  ngOnInit() {
    this.getStaffList();
  }

  getStaffList(): void {
    this.staffService.getStaffAll()
      .subscribe(sl => this.staffList = sl);
  }

  onSelect(staff: StaffDTO): void {
    this.selectedStaff = staff;
  }

}
