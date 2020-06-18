cd src

if [ ! -d "./lexical-analysis" ]; then
  mkdir lexical-analysis
fi

if [ ! -e "./lexical-analysis/out/lexical.xml" ]; then
  wget https://github.com/wesleyburlani/lexical-analysis/archive/master.zip
  unzip master.zip
  rm -Rf lexical-analysis/*
  mv  lexical-analysis-master/* lexical-analysis
  rm -R lexical-analysis-master
  rm master.zip
fi

cd ..