import { Component, OnInit, Input } from '@angular/core';
import { ResponsibilityDTO, ResponsibilitiesService } from '../generated-services';
import { MessageboxComponent } from '../popups/messagebox-component';
import { throwError } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-responsibilities',
  templateUrl: './responsibilities.component.html',
  styleUrls: ['./responsibilities.component.css']
})
export class ResponsibilitiesComponent implements OnInit {

  @Input() responsibility: ResponsibilityDTO;

  constructor(
    public dialog: MatDialog,
    private service: ResponsibilitiesService) { }

  ngOnInit() {
    this.startLoad();
  }

  startLoad(): void {
    this.service.getMyResponsibility()
      .subscribe(
        result => this.responsibility = result[0],
        error => this.handleError(error)
      );
  }

  signoff(): void {

    if (this.responsibility.currentUserIsManager)
      this.responsibility.managerSignoff = new Date();
    else
      this.responsibility.employeeSignoff = new Date();

    this.save('Signoff Successful!', 'You have successfully signed off these responsibilities.');
    
  }

  save(title?: string, message?: string): void {
    this.service.putResponsibility(this.responsibility.id, this.responsibility)
      .subscribe(
        () => {
          this.dialog.open(MessageboxComponent, {
            width: '340px',
            data: {
              title: title || 'Success!',
              message: message || 'Your responsibilities were saved successfully.'
            }
          });
        },
        error => this.handleError(error)
      );
  }

  handleError(error) {

    let errorMessage = '';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`; // client-side error
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
