using System;
using System.Collections.Generic;

namespace TP1_Gestion_des_comptes
{
    class Compte
    {
        private double solde;
        private string proprietaire;
        private List<Operation> listOperation;

        public Compte(string proprietaire)
        {
            this.proprietaire = proprietaire;
            this.solde = 0;
            this.listOperation = new List<Operation>();
        }

        public double GetSolde()
        {
            return this.solde;
        }

        public void SetSolde(double solde)
        {
            this.solde = solde;
        }

        public string GetProprietaire()
        {
            return this.proprietaire;
        }

        public void SetProprietaire(string prop)
        {
            this.proprietaire = prop;
        }

        public List<Operation> GetListOperation()
        {
            return this.listOperation;
        }

        public virtual void Crediter(double somme)
        {
            this.solde += somme;
            this.listOperation.Add(new Operation(somme, "Crédit"));
        }

        public void Crediter(double somme,Compte c)
        {
            c.SetSolde(c.GetSolde() + somme);
            c.listOperation.Add(new Operation(somme, "Crédit"));
            this.solde -= somme;
            this.listOperation.Add(new Operation(-somme, "Débit"));
        }

        public void Debiter(double somme)
        {
            this.solde -= somme;
            this.listOperation.Add(new Operation(-somme, "Débit"));
        }

        public void Debiter(double somme, Compte c)
        {
            c.SetSolde(c.GetSolde() - somme);
            c.listOperation.Add(new Operation(-somme , "Débit"));
            this.solde += somme;
            this.listOperation.Add(new Operation(somme, "Crédit"));
        }

        public virtual void Resume()
        {
            foreach(Operation o in this.listOperation)
            {
                string montant = o.GetMontant() > 0 ? "+" + o.GetMontant() : o.GetMontant().ToString();
                Console.WriteLine("          " + montant);
            }
        }

        public string AfficheCompte()
        {
            return this.proprietaire + " : " + this.solde;
        }

        public virtual void Afficher()
        {

        }
    }

    class Courant : Compte
    {
        private readonly double decouvert;

        public Courant(string proprietaire, double decouvert) : base(proprietaire)
        {
            this.decouvert = decouvert;
        }

        public double GetDecouvert()
        {
            return this.decouvert;
        }
        
        public override void Resume()
        {
            Console.WriteLine("Résumé du compte de " + base.GetProprietaire());
            Console.WriteLine("*********************************************");
            Console.WriteLine("Compte courant de " + base.GetProprietaire());
            Console.WriteLine("         Solde : " + base.GetSolde());
            Console.WriteLine("         Découvert autorisé : " + this.decouvert);
            Console.WriteLine("\n\n");
            Console.WriteLine("Opérations : ");
            base.Resume();
            Console.WriteLine("*********************************************\n\n");
        }

        public override void Afficher()
        {
            Console.WriteLine("Solde compte courant de " + base.AfficheCompte() );
        }
    }

    class Epargne : Compte
    {
        private readonly double taux;

        public Epargne(string proprietaire, double taux) : base(proprietaire)
        {
            if(taux > 1)
            {
                this.taux = 1;
            } else if(taux >= 0)
            {
                this.taux = taux;
            } 
            else
            {
                this.taux = 0;
            }
            
        }

        public double GetTaux()
        {
            return this.taux;
        }

        public override void Crediter(double somme)
        {
            base.SetSolde(base.GetSolde() + somme + somme * this.taux);
            base.GetListOperation().Add(new Operation(somme, "Crédit"));
        }

        public override void Resume()
        {
            Console.WriteLine("Résumé du compte de " + base.GetProprietaire());
            Console.WriteLine("##############################################");
            Console.WriteLine("Compte épargne de " + base.GetProprietaire());
            Console.WriteLine("         Solde : " + base.GetSolde());
            Console.WriteLine("         Taux : " + this.taux);
            Console.WriteLine("\n\n");
            Console.WriteLine("Opérations : ");
            base.Resume();
            Console.WriteLine("#############################################\n\n");
        }

        public override void Afficher()
        {
            Console.WriteLine("Solde compte épargne de " + base.AfficheCompte());
        }
    }

    class Operation
    {
        private double montant { get; set; }
        private string typeCompte { get; set; }

        public Operation(double montant, string type)
        {
            this.montant = montant;
            this.typeCompte = type;
        }

        public double GetMontant()
        {
            return this.montant;
        }

        public void SetMontant(double montant)
        {
            this.montant = montant;
        }

        public string GetTypeCompte()
        {
            return this.typeCompte;
        }

        public void SetTypeCompte(String type)
        {
            this.typeCompte = type;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Compte nicolasC = new Courant("Nicolas",2000);
            Compte nicolasE = new Epargne("Nicolas", 0.02);
            Compte jeremieC = new Courant("Jérémie", 500);
            nicolasC.Crediter(100);
            nicolasC.Debiter(50);
            nicolasC.Crediter(20, nicolasE);
            nicolasE.Crediter(100);
            nicolasE.Crediter(20, nicolasC);
            jeremieC.Debiter(500);
            jeremieC.Crediter(200, nicolasC);

            nicolasC.Afficher();
            nicolasE.Afficher();
            jeremieC.Afficher();

            nicolasC.Resume();
            nicolasE.Resume();
        }
    }
}
