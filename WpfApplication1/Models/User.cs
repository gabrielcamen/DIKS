namespace WpfApplication1.Models
{
    public class User : IBaseUser
    {
        private string _name;
    
        public string Name  
        { 
            get => _name;
            set => _name = value; 
        }
        
        public User()
        {
            
        }

        public User(string name)
        {
            Name = name;
        }
    }

    public interface IBaseUser
    {
    }
}
