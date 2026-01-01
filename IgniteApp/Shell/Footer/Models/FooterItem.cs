using IT.Tangdao.Framework.Abstractions;

namespace IgniteApp.Shell.Footer.Models
{
    public class FooterItem : ITangdaoCloneable<FooterItem>
    {
        public string Title { get; set; }

        public FooterItem Clone()
        {
            return new FooterItem() { Title = this.Title };
        }
    }
}