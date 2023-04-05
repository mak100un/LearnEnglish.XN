using System;
using CoreGraphics;
using Foundation;
using LearnEnglish.XN.Core.Definitions.Extensions;
using Swordfish.NET.Collections.Auxiliary;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewLayouts;

public class MessagesFlowLayout : UICollectionViewFlowLayout
{
    public override UICollectionViewScrollDirection ScrollDirection => UICollectionViewScrollDirection.Vertical;

    public override nfloat MinimumLineSpacing => 0;

    public override nfloat MinimumInteritemSpacing => 0;

    public override CGSize EstimatedItemSize => AutomaticSize;
}
