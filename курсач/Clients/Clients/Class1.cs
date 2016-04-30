using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clients
{
    public class Client
    {
       public int number_person_card;
        public string FIO;
        public string phone_number;
        public string email;
        public string address;
        public int bonus;
        public int discount;
        public int parent_personal_card;
        public int child_personal_card;
        public Client(int id, string fio, string phone, string e_mail, string addres, int bonuses, int dickount, int parent_card, int child_card)
        {
            number_person_card = id;
            FIO = fio;
            phone_number = phone;
            email = e_mail;
            address = addres;
            bonus = bonuses;
            discount = dickount;
            parent_personal_card = parent_card;
            child_personal_card = child_card;
        }
        public override string ToString()
        {
            return number_person_card.ToString() + " " + FIO + " " + phone_number.ToString()
                + " " + email + " " + address + " " + bonus.ToString() + " " + discount.ToString() + "%" + " "
                + parent_personal_card + " " + child_personal_card;

        }
    }
}
