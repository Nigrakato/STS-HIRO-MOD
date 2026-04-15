using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hiro.Scripts.Cards
{
    public interface IOption
    {
        Task OnOptionChosen(PlayerChoiceContext choiceContext, CardPlay cardPlay);
    }
}