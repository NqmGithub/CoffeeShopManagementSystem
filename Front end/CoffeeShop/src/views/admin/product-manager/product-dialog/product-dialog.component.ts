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
import { ApiService } from '../../../../Api/api.service';
import { CreateProduct } from '../../../../Interfaces/createProduct';
import { MatIconModule } from '@angular/material/icon';
import { Product } from '../../../../Interfaces/product';
@Component({
  selector: 'app-add-product-dialog',
  standalone: true,
  imports: [
    MatDialogModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatNativeDateModule,
    MatRadioButton,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule
  ],
  templateUrl: './product-dialog.component.html',
  styleUrl: './product-dialog.component.scss'
})
export class ProductDialogComponent implements OnInit {  
  header:string = "Add Product";
  fileToUpload: File| null = null;
  imageUrl: any;
  fileExtension: string = '';
  listCategoryName:string[]=[];
  isProductNameExist:boolean = false;
  id:string ='';

  productNameFormControl = new FormControl('',[Validators.required]);
  categoryNameFormControl = new FormControl(this.listCategoryName[0]);
  priceFormControl = new FormControl(0,[Validators.required,Validators.min(0)])
  quantityFormControl = new FormControl(0,[Validators.required,Validators.min(0)])
  descriptionFormControl = new FormControl();
  selectedStatusFormControl = new FormControl('Active');

  createProduct: CreateProduct;
  
  constructor(@Inject(MAT_DIALOG_DATA) public data: Product, private dialogRef: MatDialogRef<ProductDialogComponent>, private apiService: ApiService) {
    if (data) {
      this.header = "Update Product";
      const product = { ...data };
      this.id = data.id;
      this.productNameFormControl.setValue(product.productName);
      this.categoryNameFormControl.setValue(product.categoryName);
      this.priceFormControl.setValue(product.price);
      this.quantityFormControl.setValue(product.quantity);
      this.descriptionFormControl.setValue(product.description);
      this.selectedStatusFormControl.setValue(product.status);
    } 
    this.loadCategoryNames();
    this.createProduct = {
      productName: this.productNameFormControl.value!,
      categoryName: this.categoryNameFormControl.value!,
      price: this.priceFormControl.value!,
      quantity: this.quantityFormControl.value!,
      thumbnail: data? data.thumbnail:"",
      description:this.descriptionFormControl.value!,
      status: data? data.status:'Active',
    };
  }
  ngOnInit() {
    this.dialogRef.updateSize('180%', '65%');

    this.productNameFormControl.valueChanges.subscribe(value => {
      this.createProduct.productName = value || ''; 
      this.createProduct.thumbnail = this.createProduct.productName+'.'+this.fileExtension;
    });
    this.categoryNameFormControl.valueChanges.subscribe(value => {
      this.createProduct.categoryName = value || ''; 
    });
    this.quantityFormControl.valueChanges.subscribe(value => {
      this.createProduct.quantity = value || 0; 
    });
    this.priceFormControl.valueChanges.subscribe(value => {
      this.createProduct.price = value || 0; 
    });
    this.descriptionFormControl.valueChanges.subscribe(value => {
      this.createProduct.description = value || null; 
    });
    this.selectedStatusFormControl.valueChanges.subscribe(value => {
      this.createProduct.status = value || 'Active'; 
    });
}

  //preview image
  handleFileInput(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input && input.files && input.files.length > 0) {
      this.fileToUpload = input.files.item(0);
      
      // check file upload
      if (this.fileToUpload) {

        //get extension file
        const fileName = this.fileToUpload.name;
        const fileExtension = fileName.split('.').pop();
        this.fileExtension = fileExtension!;
        this.createProduct.thumbnail = this.createProduct.productName+'.'+this.fileExtension;

        let reader = new FileReader();
        reader.onload = (event: any) => {
          this.imageUrl = event.target.result;
        }
        reader.readAsDataURL(this.fileToUpload);
      } else {
        console.error("No file found or file is invalid.");
      }
    } else {
      console.error("File list is empty or input.files is null.");
    }
  }

  OnUpdate(){
    if (this.productNameFormControl.invalid ||
      this.priceFormControl.invalid ||
      this.quantityFormControl.invalid){
        return;
      }

      console.log(this.createProduct.productName)
    const product: Product = {
      id: this.id,
    productName: this.createProduct.productName,
    categoryName: this.createProduct.categoryName,
    price: this.createProduct.price,
    quantity: this.createProduct.quantity,
    thumbnail: this.createProduct.thumbnail,
    description:this.createProduct.description,
    status: this.createProduct.status
    }
    this.dialogRef.close(product); 

    this.uploadImage();
  }

  async OnAdd() {
    await this.checkProductNameExist();
    if (this.productNameFormControl.invalid ||
      this.priceFormControl.invalid ||
      this.quantityFormControl.invalid ||
      this.isProductNameExist){
        return;
      }
    this.dialogRef.close(this.createProduct); 

    this.uploadImage();
  }

  uploadImage(){
//add image to storage
if (!this.fileToUpload) {
  return;
}
const formData = new FormData();
formData.append('file', this.fileToUpload, this.fileToUpload.name);
this.apiService.uploadImage(this.createProduct.thumbnail,formData).subscribe({
  next: (event) => {
},
error: (err: HttpErrorResponse) => console.log(err)
});
  }

  onCancel() {
    this.dialogRef.close();
  }

  loadCategoryNames(){
    this.apiService.getAllCateogryNames().subscribe(
      (data : string[]) => {
        this.listCategoryName = data;
        this.categoryNameFormControl.setValue(this.listCategoryName[0]);
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }

  checkProductNameExist(): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this.apiService.checkProductNameExist(this.createProduct.productName).subscribe(
        (data: boolean) => {
          this.isProductNameExist = data;
          resolve(data);
        },
        (error) => {
          console.error('Error checking product name:', error);
          reject(error);
        }
      );
    });
  }

  getImage(name: string){
    return `https://localhost:44344/Resources/Images/${name}`;
  }
}
