import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { Item } from 'src/app/models/Item';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-add-item',
  templateUrl: './add-item.component.html',
  styleUrls: ['./add-item.component.scss']
})
export class AddItemComponent {
  name!: string;
  quantity!: number;
  price!: number;
  constructor(private usersService: UsersService, private router: Router,
    private accountService : AccountService) {}

  ngOnInit(): void {
  }

  onSubmit(productForm: NgForm){
    const userId = this.accountService.getUserId();
    productForm.control.markAllAsTouched();
    if (productForm.valid) {
      const item: Item = productForm.value;
      
      this.usersService.addItemForUser(userId,item).subscribe({
        next: () => {
          this.router.navigateByUrl("/items");
          setTimeout(() => {
            alert('Prodotto registrato con successo');
          }, 200);
        },
        error: (error) => {
          console.error('Errore durante la registrazione:', error);
          setTimeout(() => {
            alert('Errore durante la registrazione');
          }, 200);
        }  
      });
    }
  }
  }
  

