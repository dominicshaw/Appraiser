import { Component, OnInit } from '@angular/core';

import { throwError as _observableThrow, of as _observableOf, throwError } from 'rxjs';

import { AppraisalDTO } from '../generated-services';
import { AppraisalsService } from '../generated-services';

import { AllCommunityModules } from '@ag-grid-community/all-modules';

import { MyLinkRendererComponent } from '../my-link-renderer.component';
import { MatDialog } from '@angular/material/dialog';
import { MessageboxComponent } from '../popups/messagebox-component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})

export class DashboardComponent implements OnInit {

  columnDefs = [
    { colId: 'name', headerName: 'Name', field: 'staff.name', sortable: true, filter: true, resizable: true },
    { colId: 'periodStart', headerName: 'Year', field: 'periodStart', sortable: true, filter: true, resizable: true, valueFormatter: dateToYearConverter },
    { colId: 'complete', headerName: 'Complete?', field: 'complete', sortable: true, filter: true, resizable: true, cellClass: 'ag-cell-center', cellRenderer: renderYesNo },
    { colId: 'overdue', headerName: 'On Time?', field: 'overdue', sortable: true, filter: true, resizable: true, cellClass: 'ag-cell-center', cellRenderer: renderOverdue },
    {
      colId: 'id', headerName: '', field: 'id', cellRendererFramework: MyLinkRendererComponent,
      cellRendererParams:
      {
        routeData: [
          { label: 'View', link: '../detail' }
        ]
      }
    }
  ];

  defaultSortModel = [
    { colId: "name", sort: "asc" },
    { colId: "periodStart", sort: "asc" }
  ];

  rowData: AppraisalDTO[];

  modules = AllCommunityModules;

  constructor(public dialog: MatDialog, private service: AppraisalsService) {
  }

  ngOnInit() {
    this.startLoad();
  }

  startLoad(): void {
    this.service.getAppraisals()
      .subscribe(
        result => this.rowData = result,
        error => this.handleError(error)
      );
  }

  onGridReady(params) {
    params.api.sizeColumnsToFit();
    params.api.setSortModel(this.defaultSortModel);
  }

  handleError(error) {

    let errorMessage = '';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`; // client-side error
    } else if (error.status == 0) {
      errorMessage = 'The service is not available. Please report the issue to IT.';
    } else if (error.status == 500) {
      errorMessage = 'An error has occurred. If the problem perists, please contact IT.';
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`; // server-side error
    }

    this.dialog.open(MessageboxComponent, {
      width: '340px',
      data: {
        title: 'Error :(',
        message: errorMessage
      }
    });

    return throwError(errorMessage);
  }
}

function dateToYearConverter(params) {
  return params.value.getFullYear();
}

function renderYesNo(params) {
  if (params.value)
    return '<i class="material-icons md-24 green">done</i>';
  return '<i class="material-icons md-24 red">clear</i>';
}

function renderOverdue(params) {
  if (!params.value)
    return '<i class="material-icons md-24 green">done</i>';
  return '<i class="material-icons md-24 red">access_alarms</i>';
}