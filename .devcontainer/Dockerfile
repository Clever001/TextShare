FROM mcr.microsoft.com/dotnet/sdk:10.0

# man pages init
RUN rm /etc/dpkg/dpkg.cfg.d/excludes && \
    rm -f /usr/bin/man && \
    dpkg-divert --quiet --remove /usr/bin/man && \
    apt-get update && \
    apt-get install -y --reinstall man-db && \
    apt-get install -y \
    man-db \
    manpages \
    manpages-dev && \

# install grpcurl and other usefull packages
    apt-get install -y \
    golang-go \ 
    less \
    zsh \
    git \
    curl \
    btop \
    neofetch \
    tree \
    tldr \
    locales  \
    nano && \
    go install github.com/fullstorydev/grpcurl/cmd/grpcurl@latest && \
    go install github.com/fullstorydev/grpcui/cmd/grpcui@latest && \

# install node
    curl -fsSL https://deb.nodesource.com/setup_24.x | bash - && \
    apt-get install -y nodejs && \

# init russian locale
    rm -rf /var/lib/apt/lists/* && \
    locale-gen ru_RU.UTF-8

ENV LANG=ru_RU.UTF-8 \
    LANGUAGE=ru_RU:ru \
    LC_ALL=ru_RU.UTF-8

# init zsh, ohmyzsh, fzf
RUN sh -c "$(curl -fsSL https://raw.githubusercontent.com/ohmyzsh/ohmyzsh/master/tools/install.sh)" "" --unattended && \
    sed -i 's/ZSH_THEME=.*/ZSH_THEME="robbyrussell"/' /root/.zshrc && \
    sed -i '$aexport PATH="$PATH:$HOME/go/bin"' ~/.zshrc && \
    git clone --depth 1 https://github.com/junegunn/fzf.git /root/.fzf && \
    /root/.fzf/install --all

WORKDIR /workspace

SHELL ["/bin/zsh", "-c"]

CMD ["/bin/zsh"]