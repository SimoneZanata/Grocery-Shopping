import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Item } from 'src/app/models/Item';
import { AccountService } from 'src/app/services/account.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-edit-item',
  templateUrl: './edit-item.component.html',
  styleUrls: ['./edit-item.component.scss']
})
export class EditItemComponent {

  name!: string;
  quantity!: number;
  price!: number;
  itemId : number =0;
  item :Item = {} as Item;
  userId: number =0;
  
  constructor(private accountService: AccountService, private router: Router,
     private usersService: UsersService,private activatedRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.itemId = this.activatedRoute.snapshot.params['itemId'];
    this.userId = this.accountService.getUserId();
  
    this.usersService.getItemfromUser(this.userId, this.itemId).subscribe({
      next: (response) => {
        this.item = response; // Assegna i valori dell'oggetto item qui
        this.name = this.item.name;
        this.price = this.item.price;
        this.quantity = this.item.quantity;
      },
      error: (error) => {
        console.error('Errore durante la registrazione:', error);
      }  
    });
  }
  onSubmit(productForm: NgForm){
    const userId = this.accountService.getUserId();
    productForm.control.markAllAsTouched();
    if (productForm.valid) {
      const item: Item = productForm.value;
      
      this.usersService.editItemForUser(userId,this.itemId,item).subscribe({
        next: () => {
          this.router.navigateByUrl("/items");
          setTimeout(() => {
            alert('Prodotto aggiornato con successo');
          }, 200);
        },
        error: (error) => {
          console.error("Errore durante l'aggiornamento", error);
          setTimeout(() => {
            alert("Errore durante l'aggiornamento");
          }, 200);
        }  
      });
    }
  }
}

    
  

 

