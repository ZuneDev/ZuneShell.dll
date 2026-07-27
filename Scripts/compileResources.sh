UIXC_CSPROJ=../libs/ZuneUIXTools/UIXC/UIXC.csproj
dotnet build "$UIXC_CSPROJ"

echo "Built UIXC, compiling resources..."

declare -a RSRCS=(
	"../ZuneShell/Resources/RCDATA ../ZuneShell/Resources/RCDATA.resx"
	"../ZuneShell/Resources/RCDATA/Marketplace ../ZuneShell/Resources/RCDATA.Marketplace.resx"
	"../libs/ZuneUIXTools/libs/MicrosoftIris/UIXcontrols/Resources/RCDATA ../libs/ZuneUIXTools/libs/MicrosoftIris/UIXcontrols/Resources/RCDATA.resx"
)

for rsrc in "${RSRCS[@]}"; do
    read -a strarr <<< "$rsrc"  # uses default whitespace IFS
    dotnet run --project $UIXC_CSPROJ --no-build --no-launch-profile \
		-- resx -i ${strarr[0]} -o ${strarr[1]}
done

