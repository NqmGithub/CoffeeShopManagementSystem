import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule,MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { AbstractControl, FormControl, FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatRadioButton } from '@angular/material/radio';
import { HttpClient, HttpErrorResponse, HttpEventType } from '@angular/common/http';
import { ApiService } from '../../../../../Api/api.service';
import { CreateCategory } from '../../../../../Interfaces/createCategory';
import { MatIconModule } from '@angular/material/icon';
import { Category } from '../../../../../Interfaces/category';
@Component({
  selector: 'app-category-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatNativeDateModule,
    // MatRadioButton,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule
  ],
  templateUrl: './category-dialog.component.html',
  styleUrl: './category-dialog.component.scss'
})
export class CategoryDialogComponent {
  header: string = "Add Category";
  fileExtension: string = '';
  isCategoryNameExist: boolean = false;
  id: string = '';

  categoryNameFormControl = new FormControl('', [Validators.required]);
  selectedStatusFormControl = new FormControl(0); // Default value is 0 (InActive)

  createCategory: CreateCategory = {  
      categoryName: '',
      status: 0,
  };

  constructor(@Inject(MAT_DIALOG_DATA) public data: Category, 
              private dialogRef: MatDialogRef<CategoryDialogComponent>, 
              private apiService: ApiService) {
    if (data) {
      this.header = "Update Category";
      const category = { ...data };
      this.id = data.id;
      this.categoryNameFormControl.setValue(category.categoryName);
      this.selectedStatusFormControl.setValue(category.status);  // Assign status as number
    } 
    this.createCategory = {
      categoryName: this.categoryNameFormControl.value!,
      status: data ? data.status : 0, 
    };
  }

  ngOnInit() {
    this.dialogRef.updateSize('180%', '65%');

    this.categoryNameFormControl.valueChanges.subscribe(value => {
      this.createCategory.categoryName = value || ''; 
    });

    this.selectedStatusFormControl.valueChanges.subscribe(value => {
      // Ensure status is correctly mapped from radio selection (1 or 0)
      this.createCategory.status = value === 1 ? 1 : 0; // Direct numeric comparison
    });
  }

  OnUpdate() {
    if (this.categoryNameFormControl.invalid) {
      return;
    }
    const category: Category = {
      id: this.id,
      categoryName: this.createCategory.categoryName,
      status: this.createCategory.status
    }
    this.dialogRef.close(category); 
  }

  OnAdd() {
     this.checkCategoryNameExist();
    if (this.categoryNameFormControl.invalid || this.isCategoryNameExist) {
      return;
    }
    this.dialogRef.close(this.createCategory); 
  }
  checkCategoryNameExist(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this.apiService.checkCategoryNameExist(this.createCategory.categoryName).subscribe(
        (data: boolean) => {
          this.isCategoryNameExist = data;
          resolve(data);
        },
        (error) => {
          console.error('Error checking category name:', error);
          reject(error);
        }
      );
    });
  }
  onCancel() {
    this.dialogRef.close();
  }
}

