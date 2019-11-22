import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule }    from '@angular/common/http';
import { MAT_LABEL_GLOBAL_OPTIONS } from '@angular/material/core';

import { AppComponent } from './app.component';

import { StaffComponent } from './staff/staff.component';
import { StaffDetailComponent } from './staff-detail/staff-detail.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppraisalComponent } from './appraisal/appraisal.component';
import { AppraisalDetailComponent } from './appraisal-detail/appraisal-detail.component';

import { ManagementComponent } from './management/management.component';
import { ReviewComponent } from './review/review.component';
import { TrainingComponent } from './training/training.component';
import { ObjectiveComponent } from './objective/objective.component';
import { ObjectivesComponent } from './objectives/objectives.component';
import { TrainingListComponent } from './training-list/training-list.component';
import { YearPromptComponent } from './popups/year-prompt-component';
import { MessageboxComponent } from './popups/messagebox-component';

import { AgGridModule } from '@ag-grid-community/angular';
import { MyLinkRendererComponent } from './my-link-renderer.component';

import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatExpansionModule } from '@angular/material/expansion';
import { DeleteAppraisalComponent } from './delete-appraisal/delete-appraisal.component';
import { ResponsibilitiesComponent } from './responsibilities/responsibilities.component';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AgGridModule.withComponents([]),
    MatSelectModule,
    MatCardModule,
    MatGridListModule,
    MatButtonModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatDialogModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatNativeDateModule,
    MatTableModule,
    MatTooltipModule,
    MatExpansionModule
  ],
  declarations: [
    AppComponent,
    MyLinkRendererComponent,
    StaffComponent,
    StaffDetailComponent,
    DashboardComponent,
    AppraisalComponent,
    AppraisalDetailComponent,
    ManagementComponent,
    ReviewComponent,
    TrainingComponent,
    ObjectiveComponent,
    ObjectivesComponent,
    TrainingListComponent,
    YearPromptComponent,
    MessageboxComponent,
    DeleteAppraisalComponent,
    ResponsibilitiesComponent
  ],
  providers: [
    {provide: MAT_LABEL_GLOBAL_OPTIONS, useValue: {float: 'auto'}}
  ],
  entryComponents: [MyLinkRendererComponent, YearPromptComponent, MessageboxComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
